import { type Customer } from "../types/Customer";

export interface CustomerData {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNum: string;
  fullName: string;
}

export const mapCustomerToData = (customer: Customer): CustomerData => ({
  id: customer.id,
  firstName: customer.firstName,
  lastName: customer.lastName,
  email: customer.email,
  phoneNum: customer.phoneNum,
  fullName: `${customer.firstName} ${customer.lastName}`,
});

export const mapCustomersToData = (customers: Customer[]): CustomerData[] =>
  customers.map(mapCustomerToData);
