/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Management                        Component : Test cases                              *
*  Assembly : Empiria.Tests.Financial.Accounts.dll       Pattern   : Unit tests                              *
*  Type     : FinancialAcccountTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for financial accounts objects.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Xunit;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Data;

namespace Empiria.Tests.Financial.Accounts {

  /// <summary>Unit tests for financial accounts objects.</summary>
  public class FinancialAcccountTests {

    #region Facts

    [Fact]
    public void Should_Create_FinancialAccount() {

      var stdAccount = TestsObjects.TryGetObject<StandardAccount>();
      var orgUnit = TestsObjects.TryGetObject<OrganizationalUnit>();

      var sut = new FinancialAccount(FinancialAccountType.Empty, stdAccount, orgUnit);

      Assert.Equal(orgUnit, sut.OrganizationalUnit);
      Assert.Equal(stdAccount.StdAcctNo, sut.AccountNo);
      Assert.Equal(stdAccount.Description, sut.Description);
      Assert.Equal(stdAccount, sut.StandardAccount);
      Assert.Equal(DateTime.Today, sut.StartDate);
      Assert.Equal(ExecutionServer.DateMaxValue, sut.EndDate);
      Assert.Equal(EntityStatus.Pending, sut.Status);
    }


    [Fact]
    public void Should_Delete_FinancialAccount() {
      FinancialAccount sut = TestsObjects.TryGetObject<FinancialAccount>();

      if (sut == null) {
        return;
      }

      sut.Delete();

      Assert.Equal(EntityStatus.Deleted, sut.Status);
    }


    [Fact]
    public void Should_Read_All_Financial_Accounts() {
      var sut = BaseObject.GetFullList<FinancialAccount>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FinancialAccount() {
      var sut = FinancialAccount.Empty;

      Assert.NotNull(sut);
      Assert.Equal(FinancialAccount.Parse("Empty"), sut);
      Assert.Equal(-1, sut.Id);
    }


    [Fact]
    public void Should_Search_Financial_Accounts_Using_Keywords() {

      string keywords = "eva";

      FixedList<FinancialAccount> sut = FinancialAccountDataService.SearchAccounts(keywords);

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Update_FinancialAccount() {
      FinancialAccount sut = TestsObjects.TryGetObject<FinancialAccount>((x) => x.Status == EntityStatus.Pending);

      if (sut == null) {
        return;
      }

      var fields = new FinancialAccountFields {
        AccountNo = "0920",
        Description = "Nuevo nombre",
      };

      var unchangedFields = new FinancialAccountFields {
        StandardAccountUID = sut.StandardAccount.UID,
        OrganizationalUnitUID = sut.OrganizationalUnit.UID,
        ProjectUID = sut.Project.UID,
      };

      sut.Update(fields);

      Assert.Equal(fields.AccountNo, sut.AccountNo);
      Assert.Equal(fields.Description, sut.Description);
      Assert.Equal(unchangedFields.StandardAccountUID, sut.StandardAccount.UID);
      Assert.Equal(unchangedFields.OrganizationalUnitUID, sut.OrganizationalUnit.UID);
      Assert.Equal(unchangedFields.ProjectUID, sut.Project.UID);
    }

    #endregion Facts

  }  // class FinancialAcccountTests

}  // namespace Empiria.Tests.Financial.Accounts
