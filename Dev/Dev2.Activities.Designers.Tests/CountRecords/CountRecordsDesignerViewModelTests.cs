/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2016 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Activities.Presentation.Model;
using Dev2.Studio.Core.Activities.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Activities.Designers.Tests.CountRecords
{
    [TestClass]
    public class CountRecordsDesignerViewModelTests
    {
        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("CountRecordsDesignerViewModel_SetRecordsetNameValue")]
        public void CountRecordsDesignerViewModel_SetRecordsetNameValue_ModelItemIsValid_RecordSetOnModelItemIsSet()
        {
            var modelItem = CreateModelItem();
            var viewModel = new TestCountRecordsDesignerViewModel(modelItem);
            const string ExcpectedVal = "[[Table_Records()]]";
            viewModel.RecordsetNameValue = ExcpectedVal;
            Assert.AreEqual(ExcpectedVal, viewModel.RecordsetName);
            Assert.IsTrue(viewModel.HasLargeView);
        }
        
        static ModelItem CreateModelItem()
        {
            return ModelItemUtils.CreateModelItem(new DsfCountRecordsetActivity());
        }
    }
}
