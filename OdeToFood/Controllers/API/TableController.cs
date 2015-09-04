using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper.Internal;
using OdeToFood.Data;
using OdeToFood.Data.Models;

namespace OdeToFood.Controllers.API
{
    public class TableController : ApiController
    {
        private readonly DataContext _dataContext;

        public TableController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Gets all tables
        /// </summary>
        /// <returns>IEnumerable of all tables</returns>
        public IEnumerable<Table> GetTables()
        {
            return _dataContext.Table.GetAll();
        }

        /// <summary>
        /// Gets a table spesified by id
        /// </summary>
        /// <param name="id">Id of the table that is being requested</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(Table))]
        public IHttpActionResult GetTable(int id)
        {
            var table = _dataContext.Table.Get(id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }

        /// <summary>
        /// Gets the tables that are avialable for the specific time
        /// </summary>
        /// <param name="dateTime">Time that is being inspected</param>
        /// <param name="restaurantId">Restaurant that is being inspected for avialable tables</param>
        /// <returns>Collection of avialable tables</returns>
        public IEnumerable<Table> GetTablesForTime(DateTime dateTime, int restaurantId)
        {
            var dateTimeCeil = RoundUp(dateTime, TimeSpan.FromMinutes(60));

            IEnumerable<Table> tables = _dataContext.Order.FindAll(o => o.TimeFrom != dateTimeCeil && o.Table.RestaurantId == restaurantId).Select(e => e.Table);
            return tables;
        }

        /// <summary>
        /// Updates an existing instance of table in the database
        /// </summary>
        /// <param name="id">Id of the table taht is being updated</param>
        /// <param name="table">An updated instance of table</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTable(int id, Table table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != table.Id)
            {
                return BadRequest();
            }
            try
            {
                _dataContext.Table.Update(table, id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Creates a new instance of table and saves it to the database
        /// </summary>
        /// <param name="table">A new table instance</param>
        /// <returns></returns>
        [ResponseType(typeof(Table))]
        public IHttpActionResult PostTable(Table table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dataContext.Table.Add(table);
            return CreatedAtRoute("DefaultApi", new { id = table.Id }, table);
        }

        /// <summary>
        /// Deletes a table from teh database
        /// </summary>
        /// <param name="id">Id of an table that is being deleted</param>
        /// <returns>Http result on an operation status</returns>
        [ResponseType(typeof(Table))]
        public IHttpActionResult DeleteTable(int id)
        {
            var table = _dataContext.Table.Get(id);
            if (table == null)
            {
                return NotFound();
            }
            _dataContext.Table.Delete(id);
            return Ok(table);
        }

        /// <summary>
        /// Checked whether table instance specified by id is already in the database
        /// </summary>
        /// <param name="id">Id of the table that is being inspected</param>
        /// <returns>True or false depending on whether table instance found or not</returns>
        private bool TableExists(int id)
        {
            return _dataContext.Table.FindAll(e => e.Id == id).Count > 0;
        }

        /// <summary>
        /// Takes a DateTime object and an interval in minutes. Ceils the minute value to the next interval spesified by TimeSpan param
        /// </summary>
        /// <param name="dateTime">DateTime object that is being rounded</param>
        /// <param name="interval">Interval that rounding should happen to</param>
        /// <returns>Rounded DateTime object</returns>
        private DateTime RoundUp(DateTime dateTime, TimeSpan interval)
        {
            return new DateTime(((dateTime.Ticks + interval.Ticks - 1) / interval.Ticks) * interval.Ticks);
        }
    }
}