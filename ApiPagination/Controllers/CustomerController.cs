using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pagination.WebApi.Contexts;
using Pagination.WebApi.Dto;
using Pagination.WebApi.Filter;
using Pagination.WebApi.Helpers;
using Pagination.WebApi.Models;
using Pagination.WebApi.Services;
using Pagination.WebApi.Wrappers;

namespace Pagination.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IUriService uriService;
        private readonly IDistributedCache distributedCache;
        public CustomerController(ApplicationDbContext context, IUriService uriService, IDistributedCache distributedCache)
        {
            this.context = context;
            this.uriService = uriService;
            this.distributedCache = distributedCache;
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "customerList";
            string serializedCustomerList;
            var customerList = new List<Customer>();
            var redisCustomerList = await distributedCache.GetAsync(cacheKey);
            if (redisCustomerList != null)
            {
                serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
                customerList = JsonConvert.DeserializeObject<List<Customer>>(serializedCustomerList);
            }
            else
            {
                customerList = await context.Customers.ToListAsync();
                serializedCustomerList = JsonConvert.SerializeObject(customerList);
                redisCustomerList = Encoding.UTF8.GetBytes(serializedCustomerList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCustomerList, options);
            }
            return Ok(customerList);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await context.Customers
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
            var totalRecords = await context.Customers.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, validFilter, totalRecords, uriService,route);
            return Ok(pagedReponse);
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
            return Ok(new Response<Customer>(customer));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto customerDto)
        {
             var cutsomerToInsert = new Customer { Contact = customerDto.Contact,Email = customerDto.Email,FirstName = customerDto.FirstName,LastName = customerDto.LastName};
             context.Customers.Add(cutsomerToInsert);
             await context.SaveChangesAsync("xxxx-xxx-xxxx");
             return Ok(new Response<Customer>(cutsomerToInsert));
        }
    }
}