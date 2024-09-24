namespace DemoSites
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Nap.SignIn;

    public class HomeController : Controller
    {
        public const string TestCaseText = @"/	/v2/home	/v2/home
/		/
/	http://google.com	http://google.com";

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            var builder = new StringBuilder();

            var testCases = GetTestCases();

            foreach (var testCase in testCases)
            {
                var output = Helper.GetAbsoluteUrl(testCase.Link);

                var pass = output.ToLower() == testCase.Output.ToLower();
                builder.AppendFormat("result:{0}\tsite:{1}\tlink:{2}\toutput:{3}\texpected:{4}\n",
                  pass,
                  testCase.Site,
                  testCase.Link,
                  output,
                  testCase.Output
                  );

                if (pass)
                {
                    continue;
                }

                Helper.GetAbsoluteUrl(testCase.Link);
            }


            return Content(builder.ToString());
        }

        private List<TestCase> GetTestCases()
        {
            var lines = TestCaseText.Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l));
            return lines.Select(l =>
            {
                var parts = l.Split(new[] { '\t' }, System.StringSplitOptions.None).Select(p => p.Trim()).ToArray();
                return new TestCase
                {
                    Site = parts[0],
                    Link = parts[1],
                    Output = parts[2]
                };

            }).ToList();
        }

        public class TestCase
        {
            public string Site { get; set; }

            public string Link { get; set; }

            public string Output { get; set; }
        }
    }
}
