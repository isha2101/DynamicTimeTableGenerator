using DynamicTimeTableGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTimeTableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        private static InputModel _inputModel;
        private static List<SubjectModel> _subjects;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InputSubjects(InputModel model)
        {
            if (model.WorkingDays < 1 || model.WorkingDays > 7 ||
                model.SubjectsPerDay < 1 || model.SubjectsPerDay >= 9 ||
                model.TotalSubjects <= 0)
            {
                ModelState.AddModelError("", "Invalid input values.");
                return View("Index");
            }

            _inputModel = model;
            ViewBag.TotalHours = model.WorkingDays * model.SubjectsPerDay;
            ViewBag.TotalSubjects = model.TotalSubjects;
            return View("SubjectEntry");
        }

        [HttpPost]
        public IActionResult GenerateTimeTable(List<SubjectModel> subjects)
        {
            int totalHours = _inputModel.WorkingDays * _inputModel.SubjectsPerDay;
            if (subjects.Sum(s => s.Hours) != totalHours)
            {
                ModelState.AddModelError("", $"Total hours of all subject must equal {totalHours}");
                ViewBag.TotalHours = totalHours;
                ViewBag.TotalSubjects = _inputModel.TotalSubjects;
                return View("SubjectEntry", subjects);
            }

            _subjects = subjects;

            var flatList = new List<string>();
            foreach (var s in subjects)
                flatList.AddRange(Enumerable.Repeat(s.SubjectName, s.Hours));

            flatList = flatList.OrderBy(x => Guid.NewGuid()).ToList();

            var timetable = new List<List<string>>();
            int index = 0;
            for (int row = 0; row < _inputModel.SubjectsPerDay; row++)
            {
                var rowList = new List<string>();
                for (int col = 0; col < _inputModel.WorkingDays; col++)
                {
                    rowList.Add(flatList[index++]);
                }
                timetable.Add(rowList);
                

            }

            var vm = new TimeTableViewModel
            {
                WorkingDays = _inputModel.WorkingDays,
                SubjectsPerDay = _inputModel.SubjectsPerDay,
                Subjects = subjects,

                Timetable = timetable
               

        };
            Console.WriteLine("Timetable Rows: " + vm.Timetable.Count);

            return View("TimeTable", vm);
        }


    }
}
