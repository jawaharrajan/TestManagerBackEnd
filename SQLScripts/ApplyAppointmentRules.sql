USE [Medcan-CMS]
GO

/****** Object:  StoredProcedure [dbo].[ApplyAppointmentRules]    Script Date: 5/30/2025 8:27:02 AM ******/
DROP PROCEDURE [dbo].[ApplyAppointmentRules]
GO

/****** Object:  StoredProcedure [dbo].[ApplyAppointmentRules]    Script Date: 5/30/2025 8:27:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =====================================================================
-- Author:		<Health Futures>
-- Create date: <2025-05-16>
-- Updated:     <2025-08-18>
-- Description:	<Apply Appointment Rules for selected Appointment Types>
-- =====================================================================
CREATE PROCEDURE [dbo].[ApplyAppointmentRules]
AS
   BEGIN

    SET NOCOUNT ON;

	BEGIN TRY

		BEGIN TRANSACTION;

			-- Declare table variable to hold new sequence-generated values
			DECLARE @Insert TABLE (
				TransactionItemId BIGINT,
				AccessionNo NVARCHAR(50),
				TransactionId INT,
				ProductId INT, 
				InvoiceId INT,
				[Description] VARCHAR(max)
			);
			
			DECLARE @RunDateTime DateTime = GETDATE();

			-- Insert with OUTPUT to capture sequence values
			INSERT INTO TransactionItem (
				TransactionItemId,
				AccessionNo,
				TransactionId,
				ProductId,
				[Description],
				DateCreated,
				Auxiliary,
				AccpacItem,
				OhipCode01,
				OhipCode02,
				OhipFacilityFee,
				OhipProfessionalFee,    
				OhipTypeId,
				SubmittedToMOH,
				InvoiceID,
				AccountId,
				UserId
			)
			OUTPUT 
				INSERTED.TransactionItemId,
				INSERTED.AccessionNo,
				INSERTED.TransactionId,
				INSERTED.ProductId,
				INSERTED.InvoiceID,
				INSERTED.[Description]
			INTO @Insert
			SELECT 
				NEXT VALUE FOR dbo.seqTransItemId AS TransactionItemId,
				CAST(NEXT VALUE FOR dbo.seqTransItemId AS NVARCHAR(50)) AS AccessionNo,
				T.TransactionId,
				AR.ProductId,
				PR.[Name],
				@RunDateTime,
				'','','','',
				0.00,0.00,1,0,
				isNull(Inv.InvoiceID, 5481),
				null,-1
			FROM Appointment A
			INNER JOIN EntityStatus ES 
				ON A.Id = ES.InstanceID AND ES.EntityTypeId = 3 
			INNER JOIN Status S 
				ON ES.StatusId = S.StatusId AND S.EntityTypeId = 3 AND S.StatusId in (1300, 1301)
			INNER JOIN AppointmentRules AR 
				ON A.AppointmentTypeId = AR.AppointmentTypeID AND AR.IsActive = 1
			INNER JOIN [Transaction] T 
				ON A.Id = T.InstanceId AND T.EntityTypeId = 3
			OUTER APPLY (
				SELECT TOP 1 InvoiceID
				FROM Invoice I
				WHERE I.TransactionId = T.TransactionId
					AND I.IsOHIPInvoice = 0
			) AS Inv
			INNER JOIN [Product] PR 
				ON AR.ProductID = PR.ProductID
			INNER JOIN Patient P WITH (NOLOCK) 
				ON A.PatientId = P.PatientId 
			WHERE NOT EXISTS (
				SELECT 1
				FROM TransactionItem TI
				WHERE TI.TransactionId = T.TransactionId
				AND TI.ProductId = AR.ProductId
				)
			AND (
					(AR.IsMale = 1 AND P.Gender = 'M')
					OR (AR.IsFemale = 1 AND P.Gender = 'F')
				)
			AND AR.IsActive = 1
			AND DATEDIFF(YEAR, P.Birthdate, GETDATE()) BETWEEN AR.AgeFrom AND AR.AgeTo
			AND A.CmsAppliedAutoProduct IS NULL
			AND A.[CreateDate] >= '2025-08-18';

		-- Insert into Activity log

			INSERT INTO ActivityLog
				(ActivityDate, SQLAction, EntityTypeId, InstanceId, EntityAction, UserEmail)
			SELECT  
				@RunDateTime,
				'Insert',
				3,
				A.Id,
				'Automated Rule added Product: ' + ISNULL(I.[Description], '') + 
				'  with Accession No: ' + ISNULL(I.AccessionNo, '') + 
				'  to AppointmentId: ' + CAST(A.Id AS VARCHAR)+
				'  InvociceId: ' + CAST(I.InvoiceId AS varchar),
				'Automated Appointment Rules'
			FROM @Insert I
			JOIN [Transaction] T ON I.TransactionId = T.TransactionId
			JOIN Appointment A ON T.InstanceId = A.ID AND T.EntityTypeId = 3
			ORDER BY A.Id, I.TransactionId, I.TransactionItemId;

		-- Update Appointment table CmsAppliedAutoProduct column to 1 to ensure they are not checked again.
		UPDATE A
			SET A.CmsAppliedAutoProduct = 1
		FROM [Appointment] A
		JOIN [Transaction] T ON T.InstanceId = A.ID AND T.EntityTypeId = 3
		JOIN @Insert I ON I.TransactionId = T.TransactionId;

		-- Optional: Review inserted data for recording in API Logs.
		SELECT  A.Id,A.[Date],TransactionItemId,I.TransactionId, ProductId, AccessionNo, [Description]  
			FROM @Insert I
			INNER JOIN [Transaction] T
				ON I.TransactionId = T.TransactionId
			INNER JOIN Appointment A
				ON T.InstanceId = A.ID AND T.EntityTypeId =3
		ORDER By A.Id, I.TransactionId,TransactionItemId,AccessionNo,ProductId;

		COMMIT TRANSACTION;

	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;

        -- Return detailed error
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH;
	  
END;

GO


