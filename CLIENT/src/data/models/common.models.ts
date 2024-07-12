export type IdNameModel<TId = string, TName = string> = {
  id: TId;
  name: TName;
};

export type GETResponseDataModel<T> = {
  data: T | undefined;
  error: string | undefined;
};

export type POSTResponseDataModel = {
  success: string | undefined;
  error: string | undefined;
};
