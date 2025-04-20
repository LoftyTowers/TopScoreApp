export {};

declare global {
  interface Window {
    runtimeConfig: {
      apiBaseUrl: string;
    };
  }
}
