/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service provider                        *
*  Type     : PaymentOrderRules                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control payment order's rules.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Provides services to control payment order's rules.</summary>
  internal class PaymentOrderRules {

    private PaymentOrder _paymentOrder;

    internal PaymentOrderRules(PaymentOrder paymentOrder) {
      _paymentOrder = paymentOrder;
    }


    internal bool CanApproveBudget() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending &&
          !_paymentOrder.HasApprovedBudget &&
          !_paymentOrder.HasActivePaymentInstruction) {
        return true;
      }

      return false;
    }


    internal bool CanCancel() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending ||
          _paymentOrder.Status == PaymentOrderStatus.Suspended ||
          _paymentOrder.Status == PaymentOrderStatus.Programmed) {
        return true;
      }

      return false;
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanExerciseBudget() {
      if (_paymentOrder.Status == PaymentOrderStatus.Payed) {
        return true;
      }

      return false;
    }


    internal bool CanGeneratePaymentInstruction() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending &&
          _paymentOrder.HasApprovedBudget &&
          !_paymentOrder.HasActivePaymentInstruction) {
        return true;
      }

      return false;
    }


    internal bool CanReset() {
      if (_paymentOrder.Status == PaymentOrderStatus.Suspended ||
          _paymentOrder.Status == PaymentOrderStatus.Programmed) {
        return true;
      }

      return false;
    }


    internal bool CanSuspend() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending ||
          _paymentOrder.Status == PaymentOrderStatus.Programmed) {
        return true;
      }

      return false;
    }


    internal bool CanUpdate() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending) {
        return true;
      }

      return false;
    }

  }  // class PaymentOrderRules

}  // namespace Empiria.Payments
