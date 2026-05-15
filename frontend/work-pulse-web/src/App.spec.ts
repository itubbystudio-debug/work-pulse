import { mount } from '@vue/test-utils';
import { describe, expect, it } from 'vitest';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import Button from 'primevue/button';
import Card from 'primevue/card';
import Divider from 'primevue/divider';
import Tag from 'primevue/tag';
import App from './App.vue';

describe('App', () => {
  it('renders the scaffold headline and stack copy', () => {
    const wrapper = mount(App, {
      global: {
        plugins: [[PrimeVue, { theme: { preset: Aura } }]],
        components: {
          PButton: Button,
          PCard: Card,
          PDivider: Divider,
          PTag: Tag
        }
      }
    });

    expect(wrapper.text()).toContain('Work Pulse');
    expect(wrapper.text()).toContain('Vue 3 + PrimeVue');
    expect(wrapper.text()).toContain('MediatR');
  });
});
