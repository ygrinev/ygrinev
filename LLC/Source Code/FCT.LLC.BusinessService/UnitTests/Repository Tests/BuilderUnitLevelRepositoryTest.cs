using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class BuilderUnitLevelRepositoryTest
    {
        private BuilderUnitLevelRepository _builderUnitLevelRepository;
        private BuilderUnitLevelMapper _builderUnitLevelMapper;
        [SetUp]
        public void SetupTestEnvironment()
        {
            _builderUnitLevelRepository = new BuilderUnitLevelRepository(new EFBusinessContext(), new BuilderUnitLevelMapper());
            _builderUnitLevelMapper = new BuilderUnitLevelMapper();
        }

        [Test]
        public void CreateBuilderUnitLevelTest()
        {
            BuilderUnitLevel builderUnitLevel = new BuilderUnitLevel() {Level = "level1", Unit = "Unit1"};
            _builderUnitLevelRepository.InsertBuilderUnitLevel(builderUnitLevel, 12);
        }

        [Test]
        public void RetrieveBuilderUnitLevelTest()
        {
            tblBuilderUnitLevel builderUnitLevel = _builderUnitLevelRepository.FindBy(1);
            Assert.NotNull(builderUnitLevel);
        }

        [Test]
        public void UpdateBuilderLegalDescriptionTest()
        {
            tblBuilderUnitLevel tblBuilderUnitLevel = _builderUnitLevelRepository.FindBy(1);
            tblBuilderUnitLevel.Level = "Levelud";
            BuilderUnitLevel builderUnitLevel = _builderUnitLevelMapper.MapToData(tblBuilderUnitLevel);
            _builderUnitLevelRepository.UpdateBuilderUnitLevel(builderUnitLevel,12);
            Assert.AreEqual(builderUnitLevel.Level, "Levelud");
        }

        [Test]
        public void DeleteBuilderUnitLevelTest()
        {
            tblBuilderUnitLevel builderUnitLevel = _builderUnitLevelRepository.FindBy(1);
           _builderUnitLevelRepository.DeleteBy(builderUnitLevel.BuilderUnitLevelId);
           Assert.IsNull(_builderUnitLevelRepository.FindBy(1));
        }

    }
}