# Webportal

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 9.1.0.

## Designs Link

[https://www.figma.com/file/3kDbhulQ3H1XUHig5vMBEn/Hospice-Design-File?node-id=57%3A0](https://www.figma.com/file/3kDbhulQ3H1XUHig5vMBEn/Hospice-Design-File?node-id=57%3A0)

## Machine Setup

**Prerequisite**

1. nodejs
2. npm
3. vscode
4. git
5. postman
6. angular cli
7. prettier (extension installed on vscode)

**To access NS iframe in local enviornment**
**Set the policy in MacOS using command terminal**
`defaults write com.microsoft.Edge LegacySameSiteCookieBehaviorEnabledForDomainList -array-add "\"[*.]hospicesource.net"\"`

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## To run specific environments build locally

Run `ng serve --configuration=dev` or `ng serve -c dev` for a development environment serve. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

Run `ng serve --configuration=local` or `ng serve -c local` for a local environment serve. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

Run `ng serve --configuration=production` or `ng serve -c production` for a production environment serve. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.
Run `npm run build -- --configuration=dev` for a development environment build. The build artifacts will be stored in the `dist/` directory.
Run `npm run build -- --configuration=e2e` for a e2e environment build. The build artifacts will be stored in the `dist/` directory.
Run `npm run build -- --configuration=local` for a local environment build. The build artifacts will be stored in the `dist/` directory.
Run `npm run build -- --configuration=admin` for a admin environment build. The build artifacts will be stored in the `dist/` directory.
Run `npm run build -- --prod` for a production environment build. The build artifacts will be stored in the `dist/` directory.

## Formating code

Run `npm run format` to format the code with recommended style guidelines

## Running linter checks

Run `ng lint` to execute the linter check.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
