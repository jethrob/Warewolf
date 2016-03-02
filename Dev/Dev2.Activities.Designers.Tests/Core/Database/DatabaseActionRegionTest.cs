﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dev2.Activities.Designers2.Core.ActionRegion;
using Dev2.Activities.Designers2.Core.Source;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.ToolBase;
using Dev2.Studio.Core.Activities.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Core;

// ReSharper disable InconsistentNaming

namespace Dev2.Activities.Designers.Tests.Core.Database
{
    [TestClass]
    public class DatabaseActionRegionTest
    {
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_Constructor")]
        public void DatabaseActionRegion_Constructor_Scenerio_Result()
        {
            //------------Setup for test--------------------------
            var src = new Mock<IDbServiceModel>();
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>());
            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()), sourceRegion);

            //------------Assert Results-------------------------
            Assert.AreEqual(25, actionRegion.CurrentHeight);
            Assert.AreEqual(25, actionRegion.MaxHeight);
            Assert.AreEqual(25, actionRegion.MinHeight);
            Assert.AreEqual(1, actionRegion.Errors.Count);
            Assert.IsTrue(actionRegion.IsVisible);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ConstructorWithSelectedAction")]
        public void DatabaseActionRegion_ConstructorWithSelectedAction_Scenerio_Result()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id };
            var action = new DbAction() { Name = "bravo" };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);
            actionRegion.SelectedAction = action;

            //------------Assert Results-------------------------
            Assert.AreEqual(action, actionRegion.SelectedAction);
            Assert.IsTrue(actionRegion.CanRefresh());
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ChangeActionSomethingChanged")]
        public void DatabaseActionRegion_ChangeActionSomethingChanged_ExpectedChange_Result()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id };
            var evt = false;
            var s2 = new DbSourceDefinition() { Id = Guid.NewGuid() };
            var action = new DbAction() { Name = "bravo" };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc, s2 });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);
            actionRegion.SomethingChanged += (a, b) => { evt = true; };
            actionRegion.SelectedAction = action;

            //------------Assert Results-------------------------
            Assert.IsTrue(evt);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ChangeActionSomethingChanged")]
        public void DatabaseActionRegion_ChangeActionSomethingChanged_RestoreRegion_Result()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id, Name = "bob" };
            var action = new DbAction() { Name = "bravo" };

            var s2 = new DbSourceDefinition() { Id = Guid.NewGuid(), Name = "bob" };
            var action1 = new DbAction() { Name = "bravo" };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc, s2 });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);

            var clone1 = new Mock<IToolRegion>();
            var clone2 = new Mock<IToolRegion>();
            var dep1 = new Mock<IToolRegion>();
            dep1.Setup(a => a.CloneRegion()).Returns(clone1.Object);

            var dep2 = new Mock<IToolRegion>();
            dep2.Setup(a => a.CloneRegion()).Returns(clone2.Object);
            actionRegion.Dependants = new List<IToolRegion> { dep1.Object, dep2.Object };
            actionRegion.SelectedAction = action;
            actionRegion.SelectedAction = action1;

            //------------Assert Results-------------------------
            dep1.Verify(a => a.RestoreRegion(clone1.Object));
            dep2.Verify(a => a.RestoreRegion(clone2.Object));
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ChangeActionSomethingChanged")]
        public void DatabaseActionRegion_ChangeActionSomethingChanged_RegionsNotRestored_Invalid()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id };
            var action = new DbAction() { Name = "bravo" };

            var s2 = new DbSourceDefinition() { Id = Guid.NewGuid() };
            var action1 = new DbAction() { Name = "bravo" };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc, s2 });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);

            var clone1 = new Mock<IToolRegion>();
            var clone2 = new Mock<IToolRegion>();
            var dep1 = new Mock<IToolRegion>();
            dep1.Setup(a => a.CloneRegion()).Returns(clone1.Object);

            var dep2 = new Mock<IToolRegion>();
            dep2.Setup(a => a.CloneRegion()).Returns(clone2.Object);
            actionRegion.Dependants = new List<IToolRegion> { dep1.Object, dep2.Object };
            actionRegion.SelectedAction = action;
            actionRegion.SelectedAction = action1;

            //------------Assert Results-------------------------
            dep1.Verify(a => a.RestoreRegion(clone1.Object), Times.Never);
            dep2.Verify(a => a.RestoreRegion(clone2.Object), Times.Never);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ChangeActionSomethingChanged")]
        public void DatabaseActionRegion_ChangeActionSomethingChanged_CloneRegion_ExpectedClone()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id };
            var s2 = new DbSourceDefinition() { Id = Guid.NewGuid() };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc, s2 });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);
            var cloned = actionRegion.CloneRegion();

            //------------Assert Results-------------------------
            Assert.AreEqual(cloned.CurrentHeight, actionRegion.CurrentHeight);
            Assert.AreEqual(cloned.MaxHeight, actionRegion.MaxHeight);
            Assert.AreEqual(cloned.IsVisible, actionRegion.IsVisible);
            Assert.AreEqual(cloned.MinHeight, actionRegion.MinHeight);
            Assert.AreEqual(((ActionRegion)cloned).SelectedAction, actionRegion.SelectedAction);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("DatabaseActionRegion_ChangeActionSomethingChanged")]
        public void DatabaseActionRegion_ChangeActionSomethingChanged_RestoreRegion_ExpectedRestore()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var act = new DsfSqlServerDatabaseActivity() { SourceId = id };
            var src = new Mock<IDbServiceModel>();
            var dbsrc = new DbSourceDefinition() { Id = id };
            var s2 = new DbSourceDefinition() { Id = Guid.NewGuid() };
            var action = new DbAction() { Name = "bravo" };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<IDbSource>() { dbsrc, s2 });

            DatabaseSourceRegion sourceRegion = new DatabaseSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSqlServerDatabaseActivity()));

            //------------Execute Test---------------------------
            ActionRegion actionRegion = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);
            // ReSharper disable once UseObjectOrCollectionInitializer
            ActionRegion actionRegionToRestore = new ActionRegion(src.Object, ModelItemUtils.CreateModelItem(act), sourceRegion);
            actionRegionToRestore.MaxHeight = 144;
            actionRegionToRestore.MinHeight = 133;
            actionRegionToRestore.CurrentHeight = 111;
            actionRegionToRestore.IsVisible = false;
            actionRegionToRestore.SelectedAction = action;

            actionRegion.RestoreRegion(actionRegionToRestore);

            //------------Assert Results-------------------------
            Assert.AreEqual(actionRegion.MaxHeight, 144);
            Assert.AreEqual(actionRegion.MinHeight, 133);
            Assert.AreEqual(actionRegion.CurrentHeight, 111);
            Assert.AreEqual(actionRegion.SelectedAction, action);
            Assert.IsFalse(actionRegion.IsVisible);
        }
    }
}