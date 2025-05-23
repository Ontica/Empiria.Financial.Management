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

using Empiria.Financial.Accounts;
using Empiria.Financial.Accounts.Adapters;

namespace Empiria.Financial {

  /// <summary>Represents a credit account.</summary>
  public class CreditAccount : FinancialAccount {
  
    #region Constructors and parsers
    internal CreditAccount() {
      // Require by Empiria FrameWork
    }

    public CreditAccount(OrganizationalUnit orgUnit, string accountNo, string name) : base(orgUnit,accountNo,name) {
      // Require by Empiria FrameWork
    }
    
    public CreditAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));
      Update(fields);
    }

    static public new CreditAccount Parse(int id) => ParseId<CreditAccount>(id);

    static public new CreditAccount Parse(string uid) => ParseKey<CreditAccount>(uid);

    static public new CreditAccount Empty => ParseEmpty<CreditAccount>();

    #endregion Constructors and parsers

    #region Properties       

    public CreditExtData CreditData {
      get {
        return new CreditExtData(this.ExtData);
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
