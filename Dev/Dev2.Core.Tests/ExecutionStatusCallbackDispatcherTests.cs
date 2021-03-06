/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2016 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using Dev2.Network.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable InconsistentNaming

namespace Dev2.Tests
{
    [TestClass]
    public class ExecutionStatusCallbackDispatcherTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Post_Where_MessageIsNull_Expect_ArgumentNullException()
        {
            ExecutionStatusCallbackDispatcher _executionStatusCallbackDispatcher = new ExecutionStatusCallbackDispatcher();
            _executionStatusCallbackDispatcher.Post(null);
        }
    }
}
