namespace Trainline.CurrencyConverter.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Service.Interface;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyConversionService _currencyConversionService;

        public CurrencyController(ICurrencyConversionService currencyConversionService)
        {
            _currencyConversionService = currencyConversionService;
        }

        [HttpGet]
        [Route("convert")]
        public async Task<ActionResult> Convert([FromQuery] CurrencyRequest currencyRequest)
        {
            try
            {
                var currencyResponse = await _currencyConversionService.ConvertAsync(currencyRequest);

                if (currencyResponse == null)
                    return StatusCode((int)HttpStatusCode.BadGateway);
                
                return Ok(currencyResponse);
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}

