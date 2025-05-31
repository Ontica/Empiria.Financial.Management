/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialAccountDto                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial accounts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Adapters {

  public class FinancialAccountDto {

    public string UID {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public NamedEntityDto FinancialAccountType {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto StandardAccount {
      get; internal set;
    }

    public NamedEntityDto OrganizationalUnit {
      get; internal set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    public AccountAttributes Attributes {
      get; internal set;
    }

    public FinancialData FinancialData {
      get; internal set;
    }


    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public NamedEntityDto Parent {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  } // class FinancialAccountDto



  public class FinancialAccountDescriptor {

    public string UID {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public string FinancialAccountTypeName {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string StandardAccountName {
      get; internal set;
    }

    public string ProjectName {
      get; internal set;
    }

    public string OrganizationalUnitName {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

    public AccountAttributes Attributes {
      get; internal set;
    }

    public FinancialData FinancialData {
      get; internal set;
    }

  }  // class FinancialAccountDescriptor

} // namespace Empiria.Financial.Adapters
