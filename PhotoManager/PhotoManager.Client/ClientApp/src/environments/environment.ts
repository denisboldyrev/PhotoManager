// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  baseUrl: 'https://localhost:5001/api',
  uploadUrl: 'https://localhost:5001/uploads',
  clientUrl: 'http://localhost:4200',
  authority: 'https://localhost:7001',
  clientId: 'angular-client',
};

export enum apiPaths {
  albums = '/albums',
  photos = '/photos'
}

export enum imgPaths {
  icons = '/icons',
  thumbnail = '/thumbnail',
  original = '/original'
}
/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
