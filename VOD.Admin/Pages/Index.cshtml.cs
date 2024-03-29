﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Admin.Models;
using VOD.Database.Services;

namespace VOD.Admin.Pages
{
    public class IndexModel : PageModel
    {
        #region Properties
        public (CardViewModel Instructors, CardViewModel Users,
                CardViewModel Courses, CardViewModel Modules,
                CardViewModel Videos, CardViewModel Downloads) Cards;
        private readonly IDbReadService _db;
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public IndexModel(IDbReadService db)
        {
            _db = db;
        }
        #endregion

        #region actions
        public void OnGet()
        {
            var (courses, downloads, instructors, modules, videos, users) = _db.Count();
            Cards = (
                Instructors: new CardViewModel
                {
                    BackgroundColor = "#9c27b0",
                    Count = instructors,
                    Description = "Instructors",
                    Icon = "person",
                    Url = "./Instructors/Index"
                },
                Users: new CardViewModel
                {
                    BackgroundColor = "#414141",
                    Count = users,
                    Description = "Users",
                    Icon = "people",
                    Url = "./Users/Index"
                },
                Courses: new CardViewModel
                {
                    BackgroundColor = "#009688",
                    Count = courses,
                    Description = "Courses",
                    Icon = "subscriptions",
                    Url = "./Courses/Index"
                },
                Modules: new CardViewModel
                {
                    BackgroundColor = "#f44336",
                    Count = modules,
                    Description = "Modules",
                    Icon = "list",
                    Url = "./Modules/Index"
                },
                Videos: new CardViewModel
                {
                    BackgroundColor = "#3f51b5",
                    Count = videos,
                    Description = "Videos",
                    Icon = "theaters",
                    Url = "./Videos/Index"
                },
                Downloads: new CardViewModel
                {
                    BackgroundColor = "#ffcc00",
                    Count = downloads,
                    Description = "Downloads",
                    Icon = "import_contacts",
                    Url = "./Downloads/Index"
                }
            );
        }
        #endregion
    }
}
