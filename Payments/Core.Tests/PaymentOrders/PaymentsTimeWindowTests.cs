/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Test cases                              *
*  Assembly : Empiria.Payments.Core.Tests.dll            Pattern   : Unit tests                              *
*  Type     : PaymentsTimeWindowTests                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for PaymentsTimeWindow type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System;

using Empiria.Payments;

namespace Empiria.Tests.Payments {

  /// <summary>Test cases for PaymentsTimeWindow type.</summary>
  public class PaymentsTimeWindowTests {

    #region Facts

    [Fact]
    public void Should_Parse_PaymentsTimeWindow() {
      var sut = PaymentsTimeWindow.Instance;

      Assert.NotNull(sut);

      Assert.Equal(0, sut.DEFAULT_START_TIME.Days);
      Assert.Equal(0, sut.DEFAULT_END_TIME.Days);

      Assert.Equal(0, sut.StartTime.Days);
      Assert.Equal(0, sut.EndTime.Days);

      Assert.Equal(DateTime.Today.Date.Add(sut.DEFAULT_START_TIME), sut.StartDateTime);
      Assert.Equal(DateTime.Today.Date.Add(sut.DEFAULT_END_TIME), sut.EndDateTime);
    }


    [Fact]
    public void Should_Update_PaymentsTimeWindow() {
      var sut = PaymentsTimeWindow.Instance;

      var startTime = TimeSpan.Parse("09:27");
      var endTime = TimeSpan.Parse("21:19");

      sut.Update(startTime, endTime);

      Assert.Equal(startTime, sut.StartTime);
      Assert.Equal(endTime, sut.EndTime);

      Assert.Equal(DateTime.Today.Date, sut.StartDateTime.Date);
      Assert.Equal(DateTime.Today.Date, sut.EndDateTime.Date);

      Assert.Equal(DateTime.Today.Date.Add(startTime), sut.StartDateTime);
      Assert.Equal(DateTime.Today.Date.Add(endTime), sut.EndDateTime);
    }

    #endregion Facts

  }  // class PaymentsTimeWindowTests

}  // namespace Empiria.Tests.Payments
