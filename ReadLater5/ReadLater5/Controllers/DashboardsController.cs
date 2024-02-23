using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace ReadLater5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly ReadLaterDataContext _context;

        public DashboardsController(ReadLaterDataContext context)
        {
            _context = context;
        }

        // GET: api/Dashboards
        [HttpGet]
        public ActionResult<IEnumerable<Dashboard>> GetDashboard(string UserID)
        {
            string sql = "";
            sql += "select t1.id, username , t2.*, t3.*\r\n";
            sql += "from AspNetUsers t1\r\n";
            sql += "outer apply(\r\n";
            sql += "	select count(*) as numbookmarks from Bookmark t3 where t1.id=t3.UserID\r\n";
            sql += ") t2\r\n";
            sql += "outer apply (\r\n";
            sql += "	select t2.url ,\r\n";
            sql += "	(\r\n";
            sql += "		select count(*) as 'NumClicks' from BookmarkClick t3 where t2.ID=t3.BookmarkID\r\n";
            sql += "	) as 'numclicks'\r\n";
            sql += "	from bookmark t2 where t2.UserID=t1.id\r\n";
            sql += "	order by numclicks DESC\r\n";
            sql += "	for json path\r\n";
            sql += ") t3(bookmarks)\r\n";
            sql += "for json path\r\n";

            string strData = "";

            System.Data.Common.DbConnection db = _context.Database.GetDbConnection();
            if(db != null)
            {
                try
                {
                    db.Open();
                    System.Data.Common.DbCommand sqlCmd = db.CreateCommand();
                    sqlCmd.CommandText = sql;
                    System.Data.Common.DbDataReader sqlReader = sqlCmd.ExecuteReader();
                    if(sqlReader != null)
                    {
                        if (sqlReader.HasRows)
                        {
                            while(sqlReader.Read())
                            {
                                strData = (string)sqlReader.GetString(0);
                            }
                        }
                    }

                } 
                catch (Exception ex)
                {
                    // log any errors here
                }

            }

            if(!string.IsNullOrEmpty(strData))
            {
                List<Dashboard> list = JsonConvert.DeserializeObject<List<Dashboard>>(strData);
                foreach(Dashboard item in list)
                {
                    item.numclicks = 0;
                    foreach(DashboardBookmark bookmarkitem in item.bookmarks)
                    {
                        item.numclicks += bookmarkitem.numclicks;
                    }
                }
                return list;
            } else
            {
                return null;
            }
            


            //return await _context.Bookmark.From
            //        .Include(x => x.Category)
            //        .Where(c => c.UserID == UserID).ToListAsync();
        }

        
       
       
    }
}
