using System;
using System.Collections.Generic;
using System.Linq;
using Generic_Disconnected_Repo;
using Generic_Disconnected_Repo_Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Generic_Disconnected_Repo_Test
{
    [TestClass]
    public class InMemoryTest : TestBase
    {
        [TestMethod]
        public void AddEncounterTest()
        {
            encounterRepo.Add(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }
        
        [TestMethod]
        public void ModifyEncounterSimpleProperty()
        {
            encounterRepo.Add(bocaRiver);
            DateTime expected = new DateTime(2000,10,10);
            bocaRiver.DateTime = expected;
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }

        [TestMethod]
        public void ModifyHomeTeamAndUpdateTest()
        {
            encounterRepo.Add(bocaRiver);
            bocaRiver.HomeTeam = tomba;
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }
        
        [TestMethod]
        public void ModifyHomeTeamDescriptionAndUpdateTest()
        {
            encounterRepo.Add(bocaRiver);
            bocaRiver.HomeTeam.Description = "desc";
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }

        [TestMethod]
        public void ModifyCommentAndUpdateTest()
        {
            encounterRepo.Add(bocaRiver);
            bocaRiver.Comments.First().Message = "comment";
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }
        
        [TestMethod]
        public void AddCommentAndUpdateTest()
        {
            encounterRepo.Add(bocaRiver);
            bocaRiver.Comments.Add(new Comment()
            {
                User = maradona,
                Id = Guid.NewGuid(),
                Message = "Msg"
            });
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }
        
        [TestMethod]
        public void RemoveCommentAndUpdateTest()
        {
            encounterRepo.Add(bocaRiver);
            bocaRiver.Comments.Remove(bocaRiver.Comments.First());
            encounterRepo.Update(bocaRiver);

            Encounter fromRepo = encounterRepo.Get(bocaRiver.Id);
            Assert.AreEqual(bocaRiver, fromRepo);
        }

        [TestMethod]
        public void AddEncountersWithSharedInformation()
        {
            encounterRepo.Add(bocaRiver);
            encounterRepo.Add(riverTomba);

            IEnumerable<Encounter> fromRepo = encounterRepo.GetAll();
            Assert.IsTrue(fromRepo.Contains(bocaRiver));
            Assert.IsTrue(fromRepo.Contains(riverTomba));
            Assert.AreEqual(2, fromRepo.Count());
        }
        
        [TestMethod]
        public void ModifySharedInformation()
        {
            encounterRepo.Add(bocaRiver);
            encounterRepo.Add(riverTomba);
            river.Description = "new";
            teamRepo.Update(river);
            
            IEnumerable<Encounter> fromRepo = encounterRepo.GetAll();
            Assert.IsTrue(encounterRepo.Get(bocaRiver.Id).HomeTeam.Description == "new");
            Assert.IsTrue(encounterRepo.Get(riverTomba.Id).AwayTeam.Description == "new");
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void AddAlreadyExistingEncounter()
        {
            encounterRepo.Add(bocaRiver);
            encounterRepo.Add(bocaRiver);
        }
        
        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void DeleteNonExistingEncounter()
        {
            encounterRepo.Delete(bocaRiver.Id);
        }
        

        [TestInitialize]
        public void Init()
        {
            base.Initialize();
        }
    }
}