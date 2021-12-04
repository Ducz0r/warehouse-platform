export class CustomerModel {
  guid: string;
  name: string;
  quantity: number;

  constructor(
    guid: string,
    name: string,
    quantity: number) {
    this.guid = guid;
    this.name = name;
    this.quantity = quantity;
  }
}
