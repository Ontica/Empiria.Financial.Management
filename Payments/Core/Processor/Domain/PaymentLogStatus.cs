/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Enumeration Type                        *
*  Type     : PaymentLogStatus                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Enumerates the status of a payment log.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor {

  /// <summary>Enumerates the status of a payment log.</summary>
  public enum PaymentLogStatus {

    Pending = 'P',

    Rejected = 'J',

    AuthorizationRequired = 'R',

    Payed = 'C',

    Deleted = 'X',

    Failed = 'F',

    All = '@',

  }  // enum PaymentLogStatus


  /// <summary>Extension methods for PaymentLogStatus enumeration.</summary>
  static public class PaymentLogStatusExtensions {

    static public string GetName(this PaymentLogStatus status) {
      switch (status) {
        case PaymentLogStatus.Pending:
          return "Pendiente";               
        case PaymentLogStatus.Rejected:
          return "Rechazada";  
        case PaymentLogStatus.AuthorizationRequired:
          return "Requiere de autorizacion";
        case PaymentLogStatus.Payed:
          return "Pagada";
        case PaymentLogStatus.Deleted:
          return "Eliminada";
        case PaymentLogStatus.Failed:
          return "Fallida";
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled payment Log status {status}.");
      }
    }


    static public NamedEntityDto MapToNamedEntity(this PaymentLogStatus status) {
      return new NamedEntityDto(status.ToString(), GetName(status));
    }

  }  // class PaymentLogStatusExtensions

}  // namespace Empiria.Payments.Processor
