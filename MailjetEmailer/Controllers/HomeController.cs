using EmailerTestApp.Models;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Mailjet.Client.Resources;
using EmailerTestApp.Repository.EmailService;

namespace EmailerTestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string _mailjetEmail;

        public HomeController(ILogger<HomeController> logger, IEmailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;

            _mailjetEmail = _configuration["SystemConfiguration:MailjetEmail"];
        }

        public IActionResult Index()
        {
            ViewBag.MailjetEmail = _mailjetEmail;
            return View();
        }


        // POST: Send-Email
        [HttpPost("home/send-email")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(EmailModel emailModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    emailModel.IsHtml = true;

                    await _emailService.SendEmail(emailModel);

                    TempData["Success"] = "Email sent successfully.";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle validation errors
                    return View("Index", emailModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending email.");

                ModelState.AddModelError("ToEmail", "Unexpected error. Please try again.");

                return View("Index", emailModel);
            }
        }

    }
}
