/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow System Core Tests                 Component : Test Helpers                            *
*  Assembly : Empiria.CashFlow.Core.Tests.dll            Pattern   : Testing external variables              *
*  Type     : ExternalVariables                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides external testing variables used for Empiria CashFlow Core module.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Contacts;
using Empiria.Parties;

using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.Tests.CashFlow {

  /// <summary>Provides external testing variables used for Empiria CashFlow Core module.</summary>
  static public class ExternalVariables {

    #region Constructors and parsers

    static ExternalVariables() {
      Preload();

      StandardAccounts = BaseObject.GetFullList<StandardAccount>();
      FinancialProjects = BaseObject.GetFullList<FinancialProject>();
      FinancialAccounts = BaseObject.GetFullList<FinancialAccount>();
    }

    static private void Preload() {
      _ = BaseObject.GetFullList<Contact>();
      _ = BaseObject.GetFullList<Party>();
      _ = BaseObject.GetFullList<CommonStorage>();
    }

    #endregion Constructors and parsers

    #region External variables

    static public FixedList<FinancialAccount> FinancialAccounts {
      get;
    }

    static public FixedList<FinancialProject> FinancialProjects {
      get;
    }

    static public FixedList<StandardAccount> StandardAccounts {
      get;
    }

    #endregion External variables

  }  // class ExternalVariables

}  // namespace Empiria.Tests.CashFlow
