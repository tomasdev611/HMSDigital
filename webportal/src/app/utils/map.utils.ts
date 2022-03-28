import * as atlas from 'azure-maps-control';
import {Map, AuthenticationType, HtmlMarker, layer, data} from 'azure-maps-control';
import {environment} from 'src/environments/environment';
const svg = `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="34" height="37" viewBox="0 0 34 37" xml:space="preserve"><rect x="0" y="0" rx="17" ry="17" width="34" height="34" fill="{color}"/></svg>`;
const siteSvg = `<img src="../../assets/images/svg/warehouse.svg" style="height:40px;width:40px;" />`;
export function initMap(element, center, zoom) {
  return new Map(element, {
    center,
    zoom,
    language: 'en-US',
    authOptions: {
      authType: AuthenticationType.subscriptionKey,
      subscriptionKey: environment.azure.mapSubscriptionKey,
    },
  });
}

export function htmlMarker(address, color, isSite = false, title?, id?) {
  return new HtmlMarker({
    htmlContent: `<div class="label-wrapper" id="${id}">${isSite ? siteSvg : svg}<div class=${
      isSite ? 'site-label' : 'order-label'
    } >${title}</div></div>`,
    color,
    position: [address?.longitude ?? 0, address?.latitude ?? 0],
  });
}

export function getLineLayer(datasource, strokeColor, strokeWidth) {
  return new layer.LineLayer(datasource, null, {
    strokeColor,
    strokeWidth,
  });
}

export function getSymbolLayer(datasource, config) {
  const {lineSpacing, placement, image, size} = config;
  return new layer.SymbolLayer(datasource, null, {
    lineSpacing,
    placement,
    iconOptions: {
      image,
      allowOverlap: true,
      anchor: 'center',
      size: size ?? 0.6,
    },
  });
}

export function mapData(longitude, latitude) {
  return new data.Feature(new data.Point([longitude, latitude]));
}

export function getListPopupTemplate(elements: Array<any>) {
  let template = `<div class="customInfobox">`;
  elements.forEach(e => {
    if (e.title && e.value) {
      template += `<div class="order-item">
      <span class="order-item-title">${e.title}: </span>${e.value}
      </div>`;
    }
  });
  template += `</div>`;
  return template;
}

export function markerPopup() {
  return new atlas.Popup({
    pixelOffset: [0, -18],
    closeButton: false,
  });
}
