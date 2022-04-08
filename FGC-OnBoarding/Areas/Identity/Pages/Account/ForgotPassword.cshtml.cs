using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FGC_OnBoarding.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FGC_OnBoarding.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<FGC_OnBoardingUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<FGC_OnBoardingUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                $"Hi " + user.Name + " ," + "<br/><br/>" + "Please reset your password by <a href=" + HtmlEncoder.Default.Encode(callbackUrl) + ">clicking here</a>." + "<br/><br/> " + "Regards," + "<br/>" + "OnBoarding Team" + "<br/>" + "FGC-Capital" + "<br/><br/>" + " <img src='https://ci4.googleusercontent.com/proxy/Ijy_ufJDMMBULhWYgAsrj0fr2fvZrDS0mpXJWi2JUg0Utl69zaiqiUy5p8-VDLMZvkl3-u_BR_ml_pcVosmrvEfoaoVAug_2WBaMtYPOTFCZGSQwiSclvJWZJFB5=s0-d-e1-ft#https://www.fgc-capital.com/apply-business-application/image/fgc_email.jpg' border='0' width='100' height='35' class='CToWUd'>") ;



                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
