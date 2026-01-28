export interface ConfirmEmailRequest {
  email: string;
  code: string;
  changedEmail?: string;
}
