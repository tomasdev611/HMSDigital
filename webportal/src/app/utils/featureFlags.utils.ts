export function isFeatureEnabled(featureName: string) {
  const featureFlags = JSON.parse(localStorage.getItem('featureFlags'));
  const feature = featureFlags?.find((item: any) => item.name === featureName);

  return feature ? feature.isEnabled : false;
}

export function saveAllFeatureFlags(features) {
  localStorage.setItem('featureFlags', JSON.stringify(features));
}

export function updateFeatureFlag(feature) {
  let featureFlags = JSON.parse(localStorage.getItem('featureFlags'));
  featureFlags = featureFlags.map((item: any) => {
    if (item.name === feature.name) {
      item = feature;
    }
    return item;
  });
  saveAllFeatureFlags(featureFlags);
}
