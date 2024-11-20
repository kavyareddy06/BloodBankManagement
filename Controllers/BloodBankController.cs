using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BloodBankManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BloodBankManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodBankController : ControllerBase
    {
        static List<BloodBank> bloodBank = new List<BloodBank>
        {
            new BloodBank{Id=1,DonorName="Radha",Age=19,BloodType="O-",PhoneNo="9848382818",Quantity=80,CollectionDate=DateTime.Now,ExpirationDate=DateTime.Now,Status="Available" },
            new BloodBank{Id=2,DonorName="Sita",Age=25,BloodType="O+",PhoneNo="8888866666",Quantity=100,CollectionDate=DateTime.Now,ExpirationDate=DateTime.Now,Status="Required"},
            new BloodBank{Id=3,DonorName="Ram",Age=50,BloodType="A-",PhoneNo="1234567890",Quantity=120,CollectionDate=DateTime.Now,ExpirationDate=DateTime.Now,Status="Available"},
            new BloodBank{Id=4,DonorName="Krish",Age=80,BloodType="AB+",PhoneNo="9878685848",Quantity=38,CollectionDate=DateTime.Now,ExpirationDate=DateTime.Now,Status="Required"}
        };


        [HttpPost]
        public ActionResult<IEnumerable<BloodBank>> Create(BloodBank b)
        {
            if (b.Age < 18)
            {
                return BadRequest($"{b.DonorName} should be more than 18years to donate");
            }
            if(b.PhoneNo.Length<10)
            {
                return BadRequest("Phone number should be of 10 digits");
            }
            b.Id = bloodBank.Any() ? bloodBank.Max(i => i.Id) + 1 : 1;
            b.CollectionDate = DateTime.Now;
            bloodBank.Add(b);

            return CreatedAtAction(nameof(GetAll), bloodBank);
        }
        [HttpGet]
        public ActionResult<IEnumerable<BloodBank>> GetAll()
        {
            return bloodBank;
        }

        [HttpGet("{id}")]
        public ActionResult<BloodBank> GetById(int id)
        {
            var donor=bloodBank.Find(i=>i.Id == id);
            if (donor == null)
            {
                return NotFound();
            }
            return donor;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,BloodBank b)
        {
            var donor = bloodBank.Find(i => i.Id == id);
            if (donor == null)
            {
                return NotFound();
            }
            donor.DonorName = b.DonorName;
            donor.Age=b.Age;
            donor.Quantity=b.Quantity;
            donor.BloodType=b.BloodType;
            donor.PhoneNo=b.PhoneNo;
            donor.CollectionDate = b.CollectionDate;
            donor.ExpirationDate = b.ExpirationDate;
            donor.Status=b.Status;
            return Content("Updated SucessFully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var donor = bloodBank.Find(i => i.Id == id);
            if (donor == null)
            {
                return NotFound();
            }
            bloodBank.Remove(donor);
            return Content($"Donor with Id  {id} Deleted Successfully");
        }
        [HttpGet("page")]
        public ActionResult<IEnumerable<BloodBank>> GetPageData(int page=1,int size = 5)
        {
            var res = bloodBank.Skip((page - 1) * size).Take(size).ToList();
            return res;
        }

        [HttpGet("searchBloodType")]
        public ActionResult<IEnumerable<BloodBank>> SearchBloodType(string? bloodType = "A+")
        {
            var res=bloodBank.AsQueryable();
            res=res.Where(i=>i.BloodType==bloodType);
            return res.ToList();

        }
        [HttpGet("searchStatus")]
        public ActionResult<IEnumerable<BloodBank>> SearchStatus(string? status = "Available")
        {
            var res = bloodBank.AsQueryable();
            res = res.Where(i => i.Status == status);
            return res.ToList();
        }
        [HttpGet("searchDonorName")]
        public ActionResult<IEnumerable<BloodBank>> SearchDonorName(string? name)
        {
            var res = bloodBank.AsQueryable();
            res = res.Where(i => i.DonorName .Contains(name,StringComparison.OrdinalIgnoreCase));
            return res.ToList();
        }

        [HttpGet("sort")]
        public ActionResult<IEnumerable<BloodBank>> SortByBloodType(string sortby="bloodtype")
        {
            var res = bloodBank.AsQueryable();
            if (sortby.ToLower() == "bloodtype")
            {
                res = res.OrderBy(i => i.BloodType);
            }
            else if(sortby.ToLower() == "collectionDate")
            {
                res = res.OrderBy(i => i.CollectionDate);
            }
            else if(sortby.ToLower()=="age")
            {
                res = res.OrderBy(i => i.Age);
            }
            else
            {
                res = res.OrderBy(i => i.DonorName);
            }
            return res.ToList();

        }
        [HttpGet("multipleFilter")]
        public ActionResult<IEnumerable<BloodBank>> MultipleFilter(string? bloodType="A+",string? status = "Available")
        {
            var res = bloodBank.AsQueryable();
            if (!string.IsNullOrEmpty(bloodType))
            {
                res=res.Where(i=>i.BloodType==bloodType);
            }
            if(!string.IsNullOrEmpty(status))
            {
                res=res.Where(i=>i.Status==status);
            }
            return res.ToList();
        }
    }
}
