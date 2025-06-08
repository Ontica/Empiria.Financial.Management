/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Ouput DTO                               *
*  Type     : ChartOfAccountsDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to a chart of accounts and its contents.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

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



  /// <summary>Output DTO with an account data with less information to be used in lists.</summary>
  public class StandardAccountDescriptor {

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty("Name")]
    public string Description {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public AccountRoleType RoleType {
      get; internal set;
    }

    public DebtorCreditorType DebtorCreditorType {
      get; internal set;
    }

    public int Level {
      get; internal set;
    }

    public bool IsLastLevel {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public bool Obsolete {
      get; internal set;
    }

  }  // class StandardAccountDescriptor

}  // namespace Empiria.Financial.Adapters
