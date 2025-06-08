/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Ouput DTO                               *
*  Type     : ChartOfAccountsDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to a chart of accounts and its contents.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Output DTO with data related to a chart of accounts and its contents.</summary>
  public class ChartOfAccountsDto {

    internal ChartOfAccountsDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<StandardAccountDescriptor> Accounts {
      get; internal set;
    }

  }  // public class ChartOfAccountsDto

}  // namespace Empiria.Financial.Adapters
