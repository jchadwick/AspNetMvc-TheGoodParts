Agenda
======

1. Demo application intro: a site made with Microsoft's practices
	* Default route(s)
	* Controllers with a bunch of actions
	* Actions using View()
	* [HttpPost]'ing Entities directly
	* Actions creating and managing their own instance of DbContext
	* Wacky, meaningless LINQ queries everywhere
	* Loosely-typed ViewData/ViewBag usage


2. The drawbacks and frustrations to following Microsoft's recommendations:
	* Large, incohesive, non-orthoganal controllers
	* Restrictive URLs (without resorting to custom routes)
	* Inability to reuse DbContext across multiple components in the same request
	* Model validation getting in the way by not matching the request
	* Lots of code in views
	* Controller actions require duplicate code for returning View vs. Json
	* Controller actions require lots of duplicate code to retrieve data for common elements (header, sidebar, etc.)
		- Not to mention, have to get this data EVERY request, when it rarely changes
	* Calling View() can lead to bugs


3. Fixing these issues
	* View() calls:
		- Just be explicit:  View("ViewName");
	* Controller actions with duplicate code for common elements:
		- Encapsulate in ChildActions
		- Throw an OutputCache attribute on it to keep from having to retrieve & rebuild it every time
		- Do the same thing in your views by creating HTML Helper extension methods
	* Controller actions require duplicate code for returning View vs. Json
		- Custom Action Filter to override/replace return result and return Json for Ajax request instead of View
	* Lots of code in views
		- Use a Presentation Model to move and encapsulate that logic
	* Model validation getting in the way because it doesn't match your UX
		- Use a Request Model with fields and validation logic for your specific request
