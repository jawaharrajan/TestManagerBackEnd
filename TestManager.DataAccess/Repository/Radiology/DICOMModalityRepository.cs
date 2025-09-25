using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.DTO;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace TestManager.DataAccess.Repository.Radiology
{
    public class DICOMModalityRepository(ApplicationDbContext _,
        ICMS_SyncTrackingRepository cms_SyncTrackingRepository,
        ISequenceRepository sequenceRepository) : GenericRepository<DICOMModality, int>(_), IDICOMModalityRepository
    {       

        public async Task<List<DICOMModalityDTO>> GetDICOMModalityAsync(DICOMModalityFilterDTO? filter = null)
        {
            var query = from d in _context.DICOMModality
                orderby d.ModalityCode, d.ProcedureCode
                select new DICOMModalityDTO
                {
                    ModalityId = d.DICOMModalityId,
                    ModalityCode = d.ModalityCode ?? string.Empty,
                    StudyDescription = d.StudyDescription ?? string.Empty,
                    ProcedureCode = d.ProcedureCode ?? string.Empty,
                    RoomCode = d.RoomCode ?? string.Empty
                };

            #region  - filtering
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.ModalityCode))
                {
                    query = query.Where(dm => dm.ModalityCode == filter.ModalityCode);
                }

                if (!string.IsNullOrEmpty(filter.RoomCode))
                {
                    query = query.Where(dm => dm.RoomCode == filter.RoomCode);
                }

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(dm =>
                        dm.StudyDescription.Contains(filter.SearchTerm));
                }
            }
            #endregion

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<DICOMModalityDTO> AddDICOMModality(DICOMModalityDTO dicomModalityDTO)
        {           
            DICOMModality modality = new()
            {              
                RoomCode = dicomModalityDTO.RoomCode,
                StudyDescription = dicomModalityDTO.StudyDescription,
                ProcedureCode = dicomModalityDTO.ProcedureCode,
                ModalityCode = dicomModalityDTO.ModalityCode,
                //IsDeleted = false
            };

            await AddAsync(modality);
            dicomModalityDTO.ModalityId = modality.DICOMModalityId;
            return dicomModalityDTO;
        }

        public async Task<DICOMModalityDTO?> UpdateDICOMModality(DICOMModalityDTO dicomModalityDTO)
        {
            //Check CMS tracking table if DICOMModality record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("DICOMModality", dicomModalityDTO.ModalityId);

            DICOMModality? modality = _context.DICOMModality
                .FirstOrDefault(
                    m => m.DICOMModalityId == tipsId);

            if (modality == null) return null;

            modality.RoomCode = dicomModalityDTO.RoomCode;
            modality.StudyDescription = dicomModalityDTO.StudyDescription;
            modality.ModalityCode = dicomModalityDTO.ModalityCode;
            modality.ProcedureCode = dicomModalityDTO.ProcedureCode;

            await _context.SaveChangesAsync();
            return dicomModalityDTO;
        }

        public async Task<bool> DeleteDICOMModality(int Id)
        {
            //Check CMS tracking table if DICOMModality record is processed in TIPS or not
            int tipsId = await cms_SyncTrackingRepository.GetTIPSId("DICOMModality", Id);

            DICOMModality? modality = _context.DICOMModality
                .FirstOrDefault(d => d.DICOMModalityId == tipsId);

            if (modality == null) return false;

            _context.Remove(modality);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<int> GetProductIDFromSequence()
        {
            int id = await sequenceRepository.GetNextValueFromSequence("dbo.sp_getDicomId");
            return id;
        }
    }
}
