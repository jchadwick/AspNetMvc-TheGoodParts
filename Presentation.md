Agenda
======

1. Demo application intro: a site made with Microsoft's practices
	* Default route(s)
		RouteConfig.cs
	* Controllers with a bunch of actions
		AuctionsController.cs
	* Actions using View()
		AuctionsController:  207
	* [HttpPost]'ing Entities directly
		AuctionsController: 156
	* Actions creating and managing their own instance of DbContext
		AuctionsController:  14
	* Wacky, meaningless LINQ queries everywhere
		AuctionsController:  43
	* Loosely-typed ViewData/ViewBag usage
		AuctionsController:  39

2. The drawbacks and frustrations to following Microsoft's recommendations:
	* Calling View() can lead to bugs
	* Lots of code in views
	* Model validation getting in the way by not matching the request
	* Controller actions require lots of duplicate code to retrieve data for common elements (header, sidebar, etc.)
		- Not to mention, have to get this data EVERY request, when it rarely changes
	* Controller actions require duplicate code for returning View vs. Json
	* Large, incohesive, non-orthoganal controllers
	* Restrictive URLs (without resorting to custom routes)


3. Fixing these issues
	* View() calls:
		- Just be explicit:  View("ViewName");
	* Lots of code in views
		- Use a Presentation Model to move and encapsulate that logic
	* Model validation getting in the way because it doesn't match your UX
		- Use a Request Model with fields and validation logic for your specific request
	* Views with duplicate/complex code for common elements:
		- Create HTML Helper extension methods
	* Controller actions with duplicate code for common elements:
		- Do the same thing in your views by creating HTML Helper extension methods
		- Encapsulate in ChildActions
		- Throw an OutputCache attribute on it to keep from having to retrieve & rebuild it every time
	* Controller actions require duplicate code for returning View vs. Json
		- Custom Action Filter to override/replace return result and return Json for Ajax request instead of View
