/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialAccountDto                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial accounts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Accounts.Adapters {

  public class FinancialAccountDto {

    public string UID {
      get; set;
    }


    public NamedEntityDto StandardAccount {
      get; set;
    }


    public NamedEntityDto Project {
      get; set;
    }


    public string Name {
      get; set;
    }


    public NamedEntityDto Parent {
      get; set;
    }
    

    public NamedEntityDto OrganizationUnit {
      get; set;
    }


    public EntityStatus Status {
      get; set;
    }

  } // Class FinancialAccountDto

} // Namespace Empiria.Financial.Accounts.Adapters