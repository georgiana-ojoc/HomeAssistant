using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;

namespace API.Controllers
{
    [Route("CheckoutOffer")]
    [ApiController]
    [AllowAnonymous]
    public class CheckoutOfferController
    {
        private readonly HomeAssistantContext _context;

        public CheckoutOfferController(HomeAssistantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<CheckoutOffer>> Get()
        {
            return await _context.CheckoutOffer.ToListAsync();
        }
    }
}