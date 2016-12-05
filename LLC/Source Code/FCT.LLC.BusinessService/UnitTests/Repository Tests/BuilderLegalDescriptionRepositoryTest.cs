using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class BuilderLegalDescriptionRepositoryTest
    {
        private BuilderLegalDescriptionRepository _builderLegalDescriptionRepository;
        [SetUp]
        public void SetupTestEnvironment()
        {
            _builderLegalDescriptionRepository = new BuilderLegalDescriptionRepository(new EFBusinessContext(), new BuilderLegalDescriptionMapper(new BuilderUnitLevelMapper()));
        }

        [Test]
        public void CreateBuilderLegalDescriptionTest()
        {
            BuilderUnitLevelCollection builderUnitLevelCollection = new BuilderUnitLevelCollection();
            BuilderLegalDescription builderLegalDescription = new BuilderLegalDescription()
            {
                BuilderLot = "builderlot",
                BuilderLegalDescriptionID = 0,
                ExtensionData = null,
                BuilderProjectReference = "buildProjRef",
                Plan = "Paln",
                Lot = "Lot",
                BuilderUnitsLevels = new BuilderUnitLevelCollection()
            };
              var result = _builderLegalDescriptionRepository.InsertBuilderLegalDescription(builderLegalDescription,2);
              Assert.AreNotEqual(result.BuilderLegalDescriptionID, 0);
        }

        [Test]
        public void UpdateBuilderLegalDescriptionTest()
        {
            tblBuilderLegalDescription tblBuilderLegalDescription =
                _builderLegalDescriptionRepository.GetAll().FirstOrDefault();
            BuilderLegalDescription builderLegalDescription =
                new BuilderLegalDescriptionMapper(new BuilderUnitLevelMapper()).MapToData(tblBuilderLegalDescription);
            builderLegalDescription.BuilderLot = "upodated builderlot 333";
            _builderLegalDescriptionRepository.UpdateBuilderLegalDescription(builderLegalDescription,2);
        }

        [Test]
        public void RetrieveBuilderLegalDescriptionTest()
        {
            tblBuilderLegalDescription tblBuilderLegalDescription = _builderLegalDescriptionRepository.FindBy(5);
                _builderLegalDescriptionRepository.GetAll().FirstOrDefault();
                Assert.AreEqual(tblBuilderLegalDescription.BuilderLot, "upodated builderlot 333");
        }

        [Test]
        public void DeleteBuilderLegalDescriptionWithBuilderUnitLevelCollectionTest()
        {
            tblBuilderLegalDescription tblBuilderLegalDescription =
                _builderLegalDescriptionRepository.GetAll().FirstOrDefault();
            int countOfRecords = _builderLegalDescriptionRepository.GetAll().Count();
            _builderLegalDescriptionRepository.DeleteBy(tblBuilderLegalDescription.BuilderLegalDescriptionID);
            Assert.AreEqual(_builderLegalDescriptionRepository.GetAll().Count(), countOfRecords-1);
        }

    }
}