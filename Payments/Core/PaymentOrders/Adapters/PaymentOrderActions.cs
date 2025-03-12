/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderActions                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Class used to set payment order actions.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Orders.Adapters {

  /// <summary>Class used to set payment order actions.</summary>
  public class PaymentOrderActions : BaseActions {

    private PaymentOrderActions() {}

    public bool CanSendToPay {
      get; internal set;
    }

    static internal PaymentOrderActions SetActions(PaymentOrderStatus status) {

      var actions = new PaymentOrderActions();

      switch (status) {
        case PaymentOrderStatus.Pending: {
          actions.CanDelete = true;
          actions.CanSendToPay = true;
          actions.CanUpdate = true;
          actions.CanEditDocuments = true;
        }
        break;
        default: {
          actions.CanDelete = false;
          actions.CanSendToPay = false;
          actions.CanUpdate = false;
          actions.CanEditDocuments = false;
        }
        break;
      }

      return actions;
    }

  } // class PaymentOrderActions

}  // namespace Empiria.Payments.Orders.Adapters
