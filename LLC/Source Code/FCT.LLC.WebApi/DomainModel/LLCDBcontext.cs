//====================================================================================================================
// Copyright (c) 2014 IdeaBlade
//====================================================================================================================
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
//====================================================================================================================
// USE OF THIS SOFTWARE IS GOVERENED BY THE LICENSING TERMS WHICH CAN BE FOUND AT
// http://cocktail.ideablade.com/licensing
//====================================================================================================================

using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DomainModel
{
    public class LLCDbContext : DbContext
    {
        public LLCDbContext()
        {
            //Database.SetInitializer<LLCDbContext>(new LLCDbInitializer());

            // DevForce already performs validation
           // Configuration.ValidateOnSaveEnabled = false;
        }

        public LLCDbContext(string connection = "Data Source=SQLPRI08DV01;Initial Catalog=LLCSIT;Persist Security Info=True;User ID=edac;Password=edac")
            : base(connection)
        {
            //Database.SetInitializer<LLCDbContext>(new LLCDbInitializer());

            // DevForce already performs validation
            //Configuration.ValidateOnSaveEnabled = false;
        }

        //public DbSet<StaffingResource> StaffingResources { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<AddressType> AddressTypes { get; set; }
        //public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        //public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        //public DbSet<Rate> Rates { get; set; }
        //public DbSet<RateType> RateTypes { get; set; }
        //public DbSet<State> States { get; set; }
        //public DbSet<WorkExperienceItem> WorkExperienceItems { get; set; }
        //public DbSet<Skill> Skills { get; set; }
        public DbSet<tblDeal> LLCDeals { get; set; }
        public DbSet<LLCMortgage> LLCMortgages { get; set; }
        public DbSet<LLCProperty> LLCProperties { get; set; }

        public DbSet<LLCNote> LLCNotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LLCDbContext>(null);
            base.OnModelCreating(modelBuilder);            
        }
    }

    //internal class LLCDbInitializer : DropCreateDatabaseIfModelChanges<LLCDbContext>
    //{
    //    protected override void Seed(LLCDbContext context)
    //    {
    //    }
    //}
}