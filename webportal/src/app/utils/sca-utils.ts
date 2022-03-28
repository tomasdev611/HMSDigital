import {environment} from 'src/environments/environment';

export function redirectToSCA(redirectUri) {
  let encodedRedirectedUri;
  if (redirectUri) {
    encodedRedirectedUri = encodeURIComponent(`${window.location.origin}${redirectUri}`);
  }
  window.location.href = `${environment.scaOrderingUrl}?ssoredirect=${encodedRedirectedUri || ''}`;
}
