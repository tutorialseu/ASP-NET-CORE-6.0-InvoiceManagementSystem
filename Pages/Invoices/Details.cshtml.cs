#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Data;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Authorization;

namespace IdentityApp.Pages.Invoices
{
    public class DetailsModel : DI_BasePageModel
    {

        public DetailsModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Invoice Invoice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Invoice = await Context.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (Invoice == null)
            {
                return NotFound();
            }

            var isCreator = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Read);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isCreator.Succeeded == false && isManager == false)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, InvoiceStatus status)
		{

            Invoice = await Context.Invoice.FindAsync(id);

            if (Invoice == null)
                return NotFound();


            var invoiceOperation = status == InvoiceStatus.Approved
                ? InvoiceOperations.Approve
                : InvoiceOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Invoice, invoiceOperation);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            Invoice.Status = status;
            Context.Invoice.Update(Invoice);

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
		}
    }
}
