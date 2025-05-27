/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : CreditAccount                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a credit account.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;


namespace Empiria.Financial {

  /// <summary>Represents a credit account.</summary>
  public class CreditAccount : FinancialAccount {

    #region Constructors and parsers

    protected CreditAccount(FinancialAccountType powertype) : base(powertype) {
      // Required by Empiria FrameWork
    }

    public CreditAccount(StandardAccount stdAccount, OrganizationalUnit orgUnit,
                         string accountNo, string description) : base(stdAccount, orgUnit) {
      Assertion.Require(accountNo, nameof(accountNo));
      Assertion.Require(description, nameof(description));

      AccountNo = accountNo;
      Description = description;
    }

    static public new CreditAccount Parse(int id) => ParseId<CreditAccount>(id);

    static public new CreditAccount Parse(string uid) => ParseKey<CreditAccount>(uid);

    static public new CreditAccount Empty => ParseEmpty<CreditAccount>();

    #endregion Constructors and parsers

    #region Properties

    public CreditData CreditData {
      get {
        return new CreditData(base.ConfigExtData);
      }
    }

    public CreditFinancialData FinancialData {
      get {
        return new CreditFinancialData(base.FinancialExtData);
      }
    }

    #endregion Properties

    #region Methods

    internal void Update(CreditExtDataFields fields) {
      Assertion.Require(fields, nameof(fields));

      CreditData.Update(fields);
    }

    #endregion Methods

  } // class CreditAccount

} // namespace Empiria.Financial
