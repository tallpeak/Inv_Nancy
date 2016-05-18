namespace Inv_Nancy
{
    using Nancy;
    using System;
    //using System.Collections.Generic;
        
    public class HomeModule : NancyModule
    {
        // TODO: move Inventory to database
        public Func<dynamic, dynamic> dump = p => { 
                string s = "";
                foreach (var k in p) { 
                    s += String.Format("{0}={1}<br>\n", k, p[k]); 
                }
                return s + p.Count.ToString();
            };
            
        public HomeModule()
        {
            // testing function
            Get["/showquerystring"] = args => Response.AsJson(new
            {
                Query = Request.Query,
                QueryString = Request.Url.Query
            });

            Get["/"] = _ => {
                string defaultExpiration = DateTime.Now.AddSeconds(5).ToString();
                return @"
            <html>
            <head><title>Inventory control</title>
            <script>
            function poller() {
                // find expired items
                var xhttp = new XMLHttpRequest();
                xhttp.onreadystatechange = function() {
                    //if (xhttp.status == 200) // xhttp.readyState == 4  
                    //{
                    document.getElementById(""expiredItems"").innerHTML = xhttp.responseText 
                    + ""<br>response code="" + xhttp.status + ""<br>readyState="" + xhttp.readyState;
                    //}
                };
                xhttp.open(""GET"", ""/expired/"", true);
                xhttp.send();
                setTimeout(poller, 3000);      
            }
            poller();
            </script>
            </head>
            <body>
            <p>Inventory control
                <p>add: 
                <form action=/add/>
                 <table>
                 <tr><td>label:</td><td><input type=text name=""label"" maxlength=40></td></tr>
                 <tr><td>expiration: </td><td><input type=text name=""expiration"" maxlength=20 value="""
                  + defaultExpiration + @"""></td></tr>
                 <tr><td>type: </td><td><input type=text name=""type"" maxlength=40></td></tr>
                 <tr><td></td><td><input type=submit></td></tr>
                 </table>
                 </form>
                 
                 <p>remove: 
                 <form action=/remove/>
                 <table>
                 <tr><td>label:</td><td><input type=text name=""label"" maxlength=40></td></tr>
                 <tr><td></td><td><input type=submit></td></tr>
                 </table>
                 </form>
                 
                 <pre id=""expiredItems""></pre>
                 
                 </body>
                 </html>
                ";
            };
            
            Get["/add/"] = p => {
                // Get["/add/{label}/{expiration}/{type}"] would result in this syntax:
                // InvItem ii = new InvItem(p["label"], p["expiration"], p["type"], 1);
                // But I'm using a web form, so:
                // Given QueryString = "label={label}&expiration={expiration}&type={type}"
                InvItem ii = new InvItem(Request.Query.label, Request.Query.expiration, Request.Query.type, 1);
                InvItem i2 = Startup.inv.add(ii);    
                return Response.AsJson(i2);
            };
            
            Get["/remove/"] = _ => {
                string lbl = Request.Query.Label; 
                bool success = Startup.inv.remove(lbl);    
                if (success) {
                    return "Item removed";
                } else {
                    return "Failed to remove item: " + lbl;
                }
            };
            
            Get["/expired/"] = _ => {
                return Response.AsJson(Startup.inv.findExpiredItems());
            };
        }
    }
}
