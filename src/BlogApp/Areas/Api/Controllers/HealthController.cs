using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Areas.Api.Controllers
{
    public class HealthController : Controller
    {
        Repositories.Repository<EF.Tables.StepInsight> StepInsightRepo = new Repositories.Repository<EF.Tables.StepInsight>();

        [HttpPost]
        [Route("api/health/send")]
        public IActionResult SendData([FromBody]Models.HealthModel model)
        {
            if (model.data.IndexOf(';') != -1)
            {
                Dictionary<string, int> stepsInsight = new Dictionary<string, int>();
                string[] stepsData = model.data.Split(';');
                foreach (string stepData in stepsData)
                {
                    if (string.IsNullOrEmpty(stepData)) continue;
                    int stepCount = int.Parse(stepData.Split('?')[0]);
                    DateTime stepDateTime = DateTime.Parse(stepData.Split('?')[1]);
                    string key = $"{stepDateTime.Month}/{stepDateTime.Day}/{stepDateTime.Year}";
                    if (!stepsInsight.ContainsKey(key))
                        stepsInsight.Add(key, stepCount);
                    else
                        stepsInsight[key] += stepCount;
                }
                foreach (string key in stepsInsight.Keys)
                {
                    var step = StepInsightRepo.First(p => p.Key == key);
                    if (step != null)
                    {
                        step.StepCount = stepsInsight[key];
                        StepInsightRepo.Update(step);
                    }
                    else
                    {
                        StepInsightRepo.Add(new EF.Tables.StepInsight()
                        {
                            Key = key,
                            Day = DateTime.Parse(key),
                            StepCount = stepsInsight[key]
                        });
                    }
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/health/get")]
        public BlogApp.Models.HealthStaticsModel Get()
        {
            var model = new BlogApp.Models.HealthStaticsModel();
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;
            model.StartDate = startDate.AddDays(1).ToString("d MMM", new CultureInfo("en-US"));
            model.EndDate = endDate.ToString("d MMM", new CultureInfo("en-US"));
            var steps = StepInsightRepo.Where(s => s.Day >= startDate & s.Day <= endDate);
            model.Steps = new List<BlogApp.Models.StepCount>();
            int totalSteps = steps.OrderByDescending(p => p.StepCount).First().StepCount;
            steps.ForEach(step =>
            {
                decimal barValue = Convert.ToDecimal(72 * step.StepCount) / Convert.ToDecimal(totalSteps);
                model.Steps.Add(new BlogApp.Models.StepCount()
                {
                    Date = step.Day.ToString("d MMM", new CultureInfo("en-US")),
                    Step = step.StepCount,
                    HealthHeight = (72 - barValue).ToString(),
                    HealthValue = barValue.ToString()
                });
            });
            return model;
        }
    }
}
