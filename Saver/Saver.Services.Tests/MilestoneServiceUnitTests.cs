using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Saver.Model;
using Saver.Repositories.Interfaces;
using Saver.Services.Implementations;
using Saver.Services.Interfaces;

namespace Saver.Services.Tests
{
    /// <summary>
    /// Tests that we can interact appropriately with
    /// the milestone service and repository layers
    /// </summary>
    [TestClass]
    public class MilestoneServiceUnitTests
    {
        public readonly Mock<IMilestoneRepository> mockRepository;

        /// <summary>
        /// Creates a new instance of the Milestone Service Unit Tests
        /// </summary>
        public MilestoneServiceUnitTests()
        {
            this.mockRepository = new Mock<IMilestoneRepository>();
        }

        /// <summary>
        /// Tests that we can return a collection of known milestones
        /// in the appropriate order from the service
        /// </summary>
        [TestMethod]
        public void ShouldReturnAKnownCollectionOfOrderedMilestones()
        {
            //Arrange
            IEnumerable<Milestone> expectedMilestones = new List<Milestone>()
            {
                new Milestone(1, 100, "Testing 1", null),
                new Milestone(7, 200, "Testing 2", null),
                new Milestone(2, 300, "Testing 3", null),
                new Milestone(4, 400, "Testing 4", null),
                new Milestone(3, 500, "Testing 5", null)
            };
            IEnumerable<Milestone> expectedOrderedMilestones = expectedMilestones.OrderBy(m => m.Id);
            mockRepository.Setup(r => r.GetAll()).Returns(expectedMilestones);

            IMilestoneService service = new MilestoneService(mockRepository.Object);

            //Act
            IEnumerable<Milestone> milestones = service.GetAllMilestones();

            //Assert
            milestones.Should().NotBeNull();
            milestones.Should().ContainInOrder(expectedOrderedMilestones);
            milestones.Count().Should().Be(expectedOrderedMilestones.Count());
        }
    }
}
