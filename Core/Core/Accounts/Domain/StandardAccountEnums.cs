/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Enumeration types                       *
*  Type     : AccountRoleType / DebtorCreditorType       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerated types for standard accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Describes the posting or summary role of an standard account.</summary>
  public enum AccountRoleType {

    /// <summary>Summary account (cuenta sumaria).</summary>
    Sumaria = 'S',

    /// <summary>Posting account (cuenta de detalle).</summary>
    Detalle = 'P',


    Undefined = 'U'

  }  // enum AccountRoleType



  /// <summary>Enumerates an account debtor/creditor type of an standard  account (naturaleza).</summary>
  public enum DebtorCreditorType {

    /// <summary>Debtor account (naturaleza deudora).</summary>
    Deudora = 'D',

    /// <summary>Creditor account (naturaleza acreedora).</summary>
    Acreedora = 'A',

    /// <summary>Debtor and Creditor account (naturaleza combinada).</summary>
    Combinada = 'C',


    Undefined = 'U',

  }  // enum DebtorCreditorType



  /// <summary>Extension methods for standard acccount enumerations.</summary>
  static public class StandardAccountEnumsExtensionMethods {

    static public NamedEntityDto MapToNamedEntity(this AccountRoleType roleType) {
      return new NamedEntityDto(roleType.ToString(), roleType.ToString());
    }

    static public NamedEntityDto MapToNamedEntity(this DebtorCreditorType debtorCreditorType) {
      return new NamedEntityDto(debtorCreditorType.ToString(), debtorCreditorType.ToString());
    }

  }  // StandardAccountEnumsExtensionMethods

} // namespace Empiria.Financial
