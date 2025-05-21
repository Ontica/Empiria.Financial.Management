/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : FinancialAcccountTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for financial accounts objects.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using System;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial.Accounts;
using Empiria.Financial.Accounts.Adapters;

using Empiria.Tests.Financial.Projects;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for financial accounts objects.</summary>
  public class FinancialAcccountTests {

    #region Facts

    [Fact]
    public void Should_Create_FinancialAccount() {

      string name = "Crédito para obra pública la Estado de Hidalgo";
      string AcctNo = "3494";
      var orgUnit = OrganizationalUnit.Parse(3);

      var sut = new FinancialAccount(orgUnit, AcctNo, name);

      Assert.Equal(orgUnit, sut.OrganizationalUnit);
      Assert.Equal(name, sut.Name);
      Assert.True(sut.AcctNo.Length != 0);
      Assert.NotNull(sut);

      Assert.True(sut.StandardAccount.IsEmptyInstance);      
      Assert.Equal(DateTime.Today, sut.StartDate);
      Assert.Equal(ExecutionServer.DateMaxValue, sut.EndDate);
      Assert.Equal(EntityStatus.Pending, sut.Status);
    }


    [Fact]
    public void Should_Delete_FinancialAccount() {
      var sut = TestingVariables.TryGetRandomNonEmpty<FinancialAccount>();
     
      if (sut == null) {
        return;
      }

      sut.Delete();

      Assert.Equal(EntityStatus.Deleted, sut.Status);
    }


    [Fact]
    public void Should_Read_All_FinancialAccount() {
      var sut = BaseObject.GetFullList<FinancialAccount>();

      Assert.NotNull(sut);
      Assert.True(sut.Count > 0);
    }


    [Fact]
    public void Should_Read_Empty_FinancialAccount() {
      var sut = FinancialAccount.Empty;

      Assert.NotNull(sut);
      Assert.Equal(-1, sut.Id);
    }


    [Fact]
    public void Should_Update_FinancialAccount() {
      var sut = TestingVariables.TryGetRandomNonEmpty<FinancialAccount>();

      if (sut == null) {
        return;
      }

      var fields = new FinancialAccountFields {
        AcctNo = "0920",
        Name = "Nuevo nombre",
      };

      var unchangedFields = new FinancialAccountFields {
        StandardAccountUID = sut.StandardAccount.UID,
        OrganizationUnitUID = sut.OrganizationalUnit.UID,
      };

      sut.Update(fields);

      Assert.Equal(fields.AcctNo, sut.AcctNo);
      Assert.Equal(fields.Name, sut.Name);
      Assert.Equal(unchangedFields.StandardAccountUID, sut.StandardAccount.UID);
      Assert.Equal(unchangedFields.OrganizationUnitUID, sut.OrganizationalUnit.UID);
    }

    #endregion Facts

  }  // class FinancialAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
