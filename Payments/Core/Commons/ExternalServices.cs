/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : External Services Layer                 *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service connector                       *
*  Type     : ExternalServices                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Connect Empiria Payments with external services providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Payments {

  /// <summary>Connect Empiria Payments with external services providers.</summary>
  static internal class ExternalServices {

    #region Services

    static public BudgetTransactionDescriptorDto ExerciseBudget(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(paymentOrder.TryGetApprovedBudget(), nameof(paymentOrder.TryGetApprovedBudget));

      var approvedBudget = paymentOrder.TryGetApprovedBudget();

      Assertion.Require(approvedBudget, "Payment order must have an approved budget.");

      var exerciseTransaction = BudgetTransaction.CreateWith(approvedBudget, BudgetOperationType.Exercise);

      exerciseTransaction.SendToAuthorization();

      exerciseTransaction.Save();

      exerciseTransaction.Authorize();

      exerciseTransaction.Save();

      exerciseTransaction.Close();

      exerciseTransaction.Save();

      return BudgetTransactionMapper.MapToDescriptor(exerciseTransaction);
    }

    #endregion Services

  }  // class ExternalServices

}  // namespace Empiria.Payments
