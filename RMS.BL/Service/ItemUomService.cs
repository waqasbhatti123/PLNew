using RMS.BL.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.BL.Dto;
using Dapper;

namespace RMS.BL.Service
{
    public class ItemUomService
    {
        private readonly Db _db;

        public ItemUomService()
        {
            _db = new Db();
        }

        public IEnumerable<ItemUomDto> Get()
        {
            try
            {
                var result = _db.Get<ItemUomDto, dynamic>("select * from Item_Uom", new { }, CommandType.Text);
                return result;

            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public ItemUomDto GetById(int uom_cd)
        {
            try
            {
                var result = _db.GetById<ItemUomDto, dynamic>("select * from Item_Uom where uom_cd = @uom_cd", new { uom_cd = uom_cd }, CommandType.Text);
                return result;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public ResponseDto Submit(ItemUomDto dto)
        {
            var response = new ResponseDto();

            if (dto.uom_cd == 0)//insert
            {
                dto.Submit = "insert";
            }
            else//update
            {
                dto.Submit = "update";
            }

            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("@Submit", dto.Submit);
            dynamicParameters.Add("@uom_Cd", dto.uom_cd);
            dynamicParameters.Add("@uom_dsc", dto.uom_dsc);

            response = _db.Submit<dynamic, ResponseDto>("SubmitItemUom", dynamicParameters, new ResponseDto(), CommandType.StoredProcedure);

            if (response != null && response.Id > 0)
            {
                response.Status = true;
                response.Message = "Record saved successfully";
            }
            else
            {
                response = new ResponseDto();

                response.Status = false;
                response.Message = "Unable to submit record at time, please try again later";
            }
            return response;
        }

    }
}
