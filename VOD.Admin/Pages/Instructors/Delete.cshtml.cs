﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;
using VOD.Database.Services;

namespace VOD.Admin.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public InstructorDTO Input { get; set; } = new InstructorDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public DeleteModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Input = await _db.SingleAsync<Instructor, InstructorDTO>(s => s.Id.Equals(id));
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new {
                    alert = "You do not have access to this page."
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = Input.Id;
            if (ModelState.IsValid)
            {
                var succeeded = await _db.DeleteAsync<Instructor>(d => d.Id.Equals(id));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Instructor: {Input.Name}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _db.SingleAsync<Instructor, InstructorDTO>(s => s.Id.Equals(id));
            return Page();
        }
        #endregion
    }
}